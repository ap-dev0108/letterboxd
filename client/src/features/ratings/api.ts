import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"
import type { Rating } from "./types"

export async function getRatingsByMovie(movieId: string) {
  const qs = new URLSearchParams({ id: movieId })
  return await apiFetch<ApiResponse<Rating[]>>(`/Rating/getRatingsByMovie?${qs.toString()}`, {
    method: "GET",
  })
}

export async function addRating(movieId: string, rating: number) {
  const qs = new URLSearchParams({ movieId, rating: String(rating) })
  return await apiFetch<ApiResponse<Rating>>(`/Rating/add-ratings?${qs.toString()}`, {
    method: "POST",
  })
}

