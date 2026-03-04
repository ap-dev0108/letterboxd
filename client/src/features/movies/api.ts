import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"
import type { Film, FilmFilter } from "./types"

export async function getAllMoviesAdmin() {
  // Backend is absolute route: GET /getAllMovies
  return await apiFetch<Film[]>("/getAllMovies", { method: "GET", auth: true })
}

export async function filterMoviesAdmin(filter: FilmFilter) {
  const params = new URLSearchParams()
  if (filter.genreId) params.set("genreId", filter.genreId)
  if (filter.releaseYear) params.set("releaseYear", filter.releaseYear)
  if (filter.minRating != null) params.set("minRating", String(filter.minRating))
  if (filter.maxRating != null) params.set("maxRating", String(filter.maxRating))

  const qs = params.toString()
  return await apiFetch<ApiResponse<Film[]>>(`/Film/filter-movies${qs ? `?${qs}` : ""}`, {
    method: "GET",
    auth: true,
  })
}

export async function getMovieDataAdmin(movieId: string) {
  const qs = new URLSearchParams({ movieId })
  return await apiFetch<ApiResponse<Film>>(`/getMovieData?${qs.toString()}`, {
    method: "GET",
    auth: true,
  })
}

export type AddMovieRequest = {
  movieTitle: string
  releaseYear: string
  runTime: string
  posterUrl?: string | null
  movieDescription?: string | null
  averageRatings?: number
  totalRatings?: number
}

export async function addMovieAdmin(req: AddMovieRequest) {
  return await apiFetch<ApiResponse<Film>>("/addMovies", {
    method: "POST",
    auth: true,
    body: JSON.stringify({
      ...req,
      averageRatings: req.averageRatings ?? 0,
      totalRatings: req.totalRatings ?? 0,
    }),
  })
}

export type UpdateMovieRequest = {
  movieId: string
  movieTitle: string
  releaseYear: string
  runTime: string
  posterUrl?: string | null
  movieDescription?: string | null
  averageRatings?: number
  totalRatings?: number
}

export async function updateMovieAdmin(req: UpdateMovieRequest) {
  const qs = new URLSearchParams({ movieId: req.movieId })
  return await apiFetch<ApiResponse<string>>(`/Film/update-movies?${qs.toString()}`, {
    method: "PUT",
    auth: true,
    body: JSON.stringify({
      ...req,
      averageRatings: req.averageRatings ?? 0,
      totalRatings: req.totalRatings ?? 0,
    }),
  })
}

export async function deleteMovieAdmin(movieId: string) {
  const qs = new URLSearchParams({ movieId })
  return await apiFetch<ApiResponse<string>>(`/Film/delete-movies?${qs.toString()}`, {
    method: "DELETE",
    auth: true,
  })
}

