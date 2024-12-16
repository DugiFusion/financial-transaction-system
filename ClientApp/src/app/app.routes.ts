import { Routes } from '@angular/router';
import { inject } from "@angular/core";
import { map } from "rxjs/operators";

export const routes: Routes = [
  {
    path: "",
    loadComponent: () => import("./components/profile/profile.component").then(m => m.ProfileComponent),
  },
  {
    path: "login",
    loadComponent: () => import("./components/login/login.component").then(m => m.LoginComponent),
    // canActivate: [
    //   () => inject(UserService).isAuthenticated.pipe(map((isAuth) => !isAuth)),
    // ],
  },
  {
    path: "transactions",
    loadComponent: () => import("./components/transactions/transactions.component").then(m => m.TransactionsComponent),
  },

];
