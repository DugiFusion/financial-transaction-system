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
FROM nginx:alpine AS runtime

# Remove the default Nginx configuration file
RUN rm /etc/nginx/conf.d/default.conf

# Copy your custom Nginx configuration file
COPY nginx.conf /etc/nginx/conf.d

# Copy Angular build files from the build stage
COPY --from=build /usr/src/app/dist/client-app/browser /usr/share/nginx/html

# Expose port 80 for the container
EXPOSE 80

# Start Nginx server
CMD ["nginx", "-g", "daemon off;"]
