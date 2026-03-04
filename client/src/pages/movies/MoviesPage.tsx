import { useMemo, useState, type ChangeEvent } from "react"
import { useQuery } from "@tanstack/react-query"
import { Link } from "react-router-dom"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { filterMoviesAdmin } from "@/features/movies/api"

export function MoviesPage() {
  const [genreId, setGenreId] = useState("")
  const [releaseYear, setReleaseYear] = useState("")
  const [minRating, setMinRating] = useState<string>("")
  const [maxRating, setMaxRating] = useState<string>("")

  const filter = useMemo(() => {
    return {
      genreId: genreId || undefined,
      releaseYear: releaseYear || undefined,
      minRating: minRating ? Number(minRating) : undefined,
      maxRating: maxRating ? Number(maxRating) : undefined,
    }
  }, [genreId, releaseYear, minRating, maxRating])

  const q = useQuery({
    queryKey: ["movies", filter],
    queryFn: async () => {
      const res = await filterMoviesAdmin(filter)
      if (!res.success) throw new Error(res.message || "Failed to load movies")
      return res.data
    },
  })

  return (
    <div className="space-y-4">
      <div className="flex items-start justify-between gap-4">
        <div>
          <h1 className="text-2xl font-semibold tracking-tight">Movies</h1>
          <p className="text-muted-foreground text-sm mt-1">
            Filters are applied dynamically based on provided fields.
          </p>
        </div>
        <Button asChild>
          <Link to="/movies/new">Add movie</Link>
        </Button>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Filters</CardTitle>
          <CardDescription>Calls `GET /Film/filter-movies`.</CardDescription>
        </CardHeader>
        <CardContent className="grid gap-4 md:grid-cols-4">
          <div className="space-y-2">
            <Label>GenreId</Label>
            <Input
              value={genreId}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setGenreId(e.target.value)}
              placeholder="guid"
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
            <Label>Min rating</Label>
            <Input
              value={minRating}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setMinRating(e.target.value)}
              placeholder="0"
            />
          </div>
          <div className="space-y-2">
            <Label>Max rating</Label>
            <Input
              value={maxRating}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setMaxRating(e.target.value)}
              placeholder="5"
            />
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Results</CardTitle>
          <CardDescription>
            {q.isLoading ? "Loading..." : q.data ? `${q.data.length} movie(s)` : ""}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {q.isError && (
            <div className="text-destructive text-sm">{(q.error as Error).message}</div>
          )}
          {!q.isLoading && !q.isError && q.data && q.data.length === 0 && (
            <div className="text-muted-foreground text-sm">No matches.</div>
          )}

          <div className="grid gap-3 md:grid-cols-2">
            {q.data?.map((m) => (
              <div
                key={m.movieId}
                className="rounded-lg border p-4 flex items-start justify-between gap-4"
              >
                <div>
                  <Link to={`/movies/${m.movieId}`} className="font-medium hover:underline">
                    {m.movieTitle}
                  </Link>
                  <div className="text-sm text-muted-foreground">
                    {m.releaseYear} • {m.runTime}
                  </div>
                </div>
                <div className="text-right text-sm">
                  <div className="text-muted-foreground">Avg</div>
                  <div className="font-medium">{m.averageRatings ?? "-"}</div>
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

