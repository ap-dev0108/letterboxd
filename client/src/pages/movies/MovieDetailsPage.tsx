import { useEffect, useState, type ChangeEvent } from "react"
import { Link, useParams } from "react-router-dom"
import { useMutation, useQuery } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { addRating, getRatingsByMovie } from "@/features/ratings/api"
import { deleteMovieAdmin, getMovieDataAdmin, updateMovieAdmin } from "@/features/movies/api"

export function MovieDetailsPage() {
  const { movieId } = useParams()
  const [rating, setRating] = useState("5")
  const [title, setTitle] = useState("")
  const [year, setYear] = useState("")
  const [runtime, setRuntime] = useState("")
  const [poster, setPoster] = useState("")
  const [desc, setDesc] = useState("")
  const [initialized, setInitialized] = useState(false)

  const id = movieId ?? ""

  const movieQ = useQuery({
    queryKey: ["movie", id],
    queryFn: async () => {
      const res = await getMovieDataAdmin(id)
      if (!res.success) throw new Error(res.message || "Failed to load movie")
      return res.data
    },
    enabled: Boolean(id),
  })

  const initFromMovie = (m: any) => {
    setTitle(m.movieTitle ?? "")
    setYear(m.releaseYear ?? "")
    setRuntime(m.runTime ?? "")
    setPoster(m.posterUrl ?? "")
    setDesc(m.movieDescription ?? "")
  }

  useEffect(() => {
    if (movieQ.data && !initialized) {
      initFromMovie(movieQ.data)
      setInitialized(true)
    }
  }, [movieQ.data, initialized])

  const ratingsQ = useQuery({
    queryKey: ["ratings", id],
    queryFn: async () => {
      const res = await getRatingsByMovie(id)
      if (!res.success) throw new Error(res.message || "Failed to load ratings")
      return res.data
    },
    enabled: Boolean(id),
  })

  const addRatingM = useMutation({
    mutationFn: async () => {
      const r = Number(rating)
      const res = await addRating(id, r)
      if (!res.success) throw new Error(res.message || "Failed to add rating")
      return res.data
    },
    onSuccess: async () => {
      toast.success("Rating added")
      await ratingsQ.refetch()
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to add rating"),
  })

  const deleteM = useMutation({
    mutationFn: async () => {
      const res = await deleteMovieAdmin(id)
      if (!res.success) throw new Error(res.message || "Failed to delete")
      return res
    },
    onSuccess: () => toast.success("Movie deleted"),
    onError: (e: any) => toast.error(e?.message ?? "Failed to delete"),
  })

  const updateM = useMutation({
    mutationFn: async () => {
      const res = await updateMovieAdmin({
        movieId: id,
        movieTitle: title,
        releaseYear: year,
        runTime: runtime,
        posterUrl: poster || null,
        movieDescription: desc || null,
      })
      if (!res.success) throw new Error(res.message || "Failed to update")
      return res
    },
    onSuccess: async () => {
      toast.success("Movie updated")
      await movieQ.refetch()
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to update"),
  })

  if (!id) {
    return (
      <Card>
        <CardHeader>
          <CardTitle>Movie</CardTitle>
          <CardDescription>Missing movie id.</CardDescription>
        </CardHeader>
      </Card>
    )
  }

  return (
    <div className="space-y-4">
      <div className="flex items-center justify-between">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Movie details</h1>
          <p className="text-muted-foreground text-sm mt-1">{id}</p>
        </div>
        <div className="flex items-center gap-2">
          <Button asChild variant="outline">
            <Link to="/movies">Back</Link>
          </Button>
          <Button
            variant="destructive"
            disabled={deleteM.isPending}
            onClick={() => deleteM.mutate()}
          >
            {deleteM.isPending ? "Deleting..." : "Delete"}
          </Button>
        </div>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Info</CardTitle>
          <CardDescription>Uses `GET /getMovieData` (admin).</CardDescription>
        </CardHeader>
        <CardContent className="text-sm">
          {movieQ.isLoading && <div className="text-muted-foreground">Loading...</div>}
          {movieQ.isError && (
            <div className="text-destructive">{(movieQ.error as Error).message}</div>
          )}
          {movieQ.data && (
            <div className="space-y-2">
              <div className="text-lg font-medium">{movieQ.data.movieTitle}</div>
              <div className="text-muted-foreground">
                {movieQ.data.releaseYear} • {movieQ.data.runTime}
              </div>
              {movieQ.data.movieDescription && (
                <div className="mt-2">{movieQ.data.movieDescription}</div>
              )}
              <div className="flex gap-6 pt-2">
                <div>
                  <div className="text-muted-foreground">Avg</div>
                  <div className="font-medium">{movieQ.data.averageRatings ?? "-"}</div>
                </div>
                <div>
                  <div className="text-muted-foreground">Total</div>
                  <div className="font-medium">{movieQ.data.totalRatings ?? "-"}</div>
                </div>
              </div>
            </div>
          )}
        </CardContent>
      </Card>

      {movieQ.data && (
        <Card>
          <CardHeader>
            <CardTitle>Edit</CardTitle>
            <CardDescription>Uses `PUT /Film/update-movies` (admin).</CardDescription>
          </CardHeader>
          <CardContent className="grid gap-4 md:grid-cols-2">
            <div className="space-y-2 md:col-span-2">
              <label className="text-sm font-medium">Title</label>
              <Input
                value={title}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setTitle(e.target.value)}
                onFocus={() => {
                  if (!title) initFromMovie(movieQ.data)
                }}
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Release year</label>
              <Input
                value={year}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setYear(e.target.value)}
                onFocus={() => {
                  if (!year) initFromMovie(movieQ.data)
                }}
              />
            </div>
            <div className="space-y-2">
              <label className="text-sm font-medium">Runtime</label>
              <Input
                value={runtime}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setRuntime(e.target.value)}
                onFocus={() => {
                  if (!runtime) initFromMovie(movieQ.data)
                }}
              />
            </div>
            <div className="space-y-2 md:col-span-2">
              <label className="text-sm font-medium">Poster URL</label>
              <Input
                value={poster}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setPoster(e.target.value)}
                onFocus={() => {
                  if (!poster) initFromMovie(movieQ.data)
                }}
              />
            </div>
            <div className="space-y-2 md:col-span-2">
              <label className="text-sm font-medium">Description</label>
              <Input
                value={desc}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setDesc(e.target.value)}
                onFocus={() => {
                  if (!desc) initFromMovie(movieQ.data)
                }}
              />
            </div>
            <div className="md:col-span-2 flex gap-2">
              <Button
                variant="outline"
                onClick={() => initFromMovie(movieQ.data)}
              >
                Reset
              </Button>
              <Button disabled={updateM.isPending} onClick={() => updateM.mutate()}>
                {updateM.isPending ? "Saving..." : "Save changes"}
              </Button>
            </div>
          </CardContent>
        </Card>
      )}

      <Card>
        <CardHeader>
          <CardTitle>Ratings</CardTitle>
          <CardDescription>User review feature via `/Rating/*`.</CardDescription>
        </CardHeader>
        <CardContent className="space-y-4">
          <div className="flex items-end gap-3">
            <div className="space-y-2">
              <label className="text-sm font-medium">Add rating (1-5)</label>
              <Input
                value={rating}
                onChange={(e: ChangeEvent<HTMLInputElement>) => setRating(e.target.value)}
                className="w-28"
              />
            </div>
            <Button disabled={addRatingM.isPending} onClick={() => addRatingM.mutate()}>
              {addRatingM.isPending ? "Submitting..." : "Submit"}
            </Button>
          </div>

          {ratingsQ.isLoading && <div className="text-muted-foreground text-sm">Loading...</div>}
          {ratingsQ.isError && (
            <div className="text-destructive text-sm">{(ratingsQ.error as Error).message}</div>
          )}
          <div className="grid gap-2 md:grid-cols-2">
            {ratingsQ.data?.map((r) => (
              <div key={r.ratingId} className="rounded-lg border p-3 text-sm">
                <div className="font-medium">Score: {r.ratingScore}</div>
                <div className="text-muted-foreground">
                  {new Date(r.createdAt).toLocaleString()}
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

