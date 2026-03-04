import { createBrowserRouter, Navigate } from "react-router-dom"

import { RootLayout } from "@/app/ui/RootLayout"
import { HomePage } from "@/pages/home/HomePage"
import { LoginPage } from "@/pages/auth/LoginPage"
import { RegisterPage } from "@/pages/auth/RegisterPage"
import { MoviesPage } from "@/pages/movies/MoviesPage"
import { ProfilePage } from "@/pages/profile/ProfilePage"
import { GenresPage } from "@/pages/genres/GenresPage"
import { RatingsPage } from "@/pages/ratings/RatingsPage"
import { MovieDetailsPage } from "@/pages/movies/MovieDetailsPage"
import { AddMoviePage } from "@/pages/movies/AddMoviePage"
import { UsersPage } from "@/pages/admin/UsersPage"
import { DebugPage } from "@/pages/debug/DebugPage"

export const router = createBrowserRouter([
  {
    path: "/",
    element: <RootLayout />,
    children: [
      { index: true, element: <HomePage /> },
      { path: "login", element: <LoginPage /> },
      { path: "register", element: <RegisterPage /> },
      { path: "movies", element: <MoviesPage /> },
      { path: "movies/new", element: <AddMoviePage /> },
      { path: "movies/:movieId", element: <MovieDetailsPage /> },
      { path: "genres", element: <GenresPage /> },
      { path: "ratings", element: <RatingsPage /> },
      { path: "profile", element: <ProfilePage /> },
      { path: "admin/users", element: <UsersPage /> },
      { path: "debug", element: <DebugPage /> },
      { path: "*", element: <Navigate to="/" replace /> },
    ],
  },
])

