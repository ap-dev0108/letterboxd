import { useState, type ChangeEvent } from "react"
import { useNavigate } from "react-router-dom"
import { useMutation } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { addMovieAdmin } from "@/features/movies/api"

export function AddMoviePage() {
  const navigate = useNavigate()
  const [movieTitle, setMovieTitle] = useState("")
  const [releaseYear, setReleaseYear] = useState("")
  const [runTime, setRunTime] = useState("")
  const [posterUrl, setPosterUrl] = useState("")
  const [movieDescription, setMovieDescription] = useState("")

  const m = useMutation({
    mutationFn: async () => {
      const res = await addMovieAdmin({
        movieTitle,
        releaseYear,
        runTime,
        posterUrl: posterUrl || null,
        movieDescription: movieDescription || null,
      })
      if (!res.success) throw new Error(res.message || "Failed to add movie")
      return res.data
    },
    onSuccess: (movie) => {
      toast.success("Movie added")
      navigate(`/movies/${movie.movieId}`)
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to add movie"),
  })

  return (
    <div className="mx-auto max-w-2xl">
      <Card>
        <CardHeader>
          <CardTitle>Add movie</CardTitle>
          <CardDescription>Uses `POST /addMovies` (admin).</CardDescription>
        </CardHeader>
        <CardContent className="grid gap-4 md:grid-cols-2">
          <div className="space-y-2 md:col-span-2">
            <Label>Title</Label>
            <Input
              value={movieTitle}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setMovieTitle(e.target.value)}
              placeholder="Movie title"
            />
          </div>
          <div className="space-y-2">
            <Label>Release year</Label>
            <Input
              value={releaseYear}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setReleaseYear(e.target.value)}
              placeholder="2024"
            />
          </div>
          <div className="space-y-2">
            <Label>Runtime</Label>
            <Input
              value={runTime}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setRunTime(e.target.value)}
              placeholder="2h 10m"
            />
          </div>
          <div className="space-y-2 md:col-span-2">
            <Label>Poster URL</Label>
            <Input
              value={posterUrl}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setPosterUrl(e.target.value)}
              placeholder="https://..."
            />
          </div>
          <div className="space-y-2 md:col-span-2">
            <Label>Description</Label>
            <Input
              value={movieDescription}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setMovieDescription(e.target.value)}
              placeholder="Short overview"
            />
          </div>
          <div className="md:col-span-2 flex gap-2">
            <Button variant="outline" onClick={() => navigate("/movies")}>
              Cancel
            </Button>
            <Button disabled={m.isPending} onClick={() => m.mutate()}>
              {m.isPending ? "Saving..." : "Add"}
            </Button>
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

