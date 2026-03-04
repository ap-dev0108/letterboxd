import { apiFetch } from "@/shared/api/http"
import type { ApiResponse } from "@/shared/api/types"
import type { Genre } from "./types"

export async function getAllGenres() {
  return await apiFetch<ApiResponse<Genre[]>>("/getAllGenre", { method: "GET" })
}

export async function addGenre(genreTitle: string) {
  return await apiFetch<ApiResponse<{ genreTitle?: string }>>("/addGenres", {
    method: "POST",
    body: JSON.stringify({ genreTitle }),
  })
}

export async function updateGenre(genreId: string, genreTitle: string) {
  const qs = new URLSearchParams({ genreID: genreId })
  return await apiFetch<ApiResponse<{ genreTitle?: string }>>(`/updateGenres?${qs.toString()}`, {
    method: "PUT",
    body: JSON.stringify({ genreId, genreTitle }),
  })
}

export async function deleteGenre(genreId: string, genreTitle?: string | null) {
  return await apiFetch<ApiResponse<{ genreTitle?: string }>>("/deleteGenre", {
    method: "DELETE",
    body: JSON.stringify({ genreId, genreTitle }),
  })
}

