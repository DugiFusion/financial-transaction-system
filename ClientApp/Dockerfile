# Stage 1: Build
FROM node:18 AS build

# Set working directory inside the container
WORKDIR /usr/src/app

# Copy package.json and package-lock.json for dependency installation
COPY package*.json ./

# Install dependencies
RUN npm install

# Copy the Angular app's source code into the container
COPY . .

# Build the Angular application
RUN npm run build:prod

# Stage 2: Runtime
FROM node:18 AS runtime

# Set working directory inside the container
WORKDIR /usr/src/app

# Install a lightweight static server
RUN npm install -g serve

# Copy Angular build files from the build stage
COPY --from=build /usr/src/app/dist/client-app/browser ./dist

# Expose port 8080 for the container
EXPOSE 8080

# Use the static server to serve the Angular app
CMD ["serve", "-s", "dist", "-l", "8080"]
