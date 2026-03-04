import { useState, type ChangeEvent } from "react"
import { useMutation, useQuery } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { addRating, getRatingsByMovie } from "@/features/ratings/api"

export function RatingsPage() {
  const [movieId, setMovieId] = useState("")
  const [rating, setRating] = useState("5")

  const q = useQuery({
    queryKey: ["ratings", movieId],
    queryFn: async () => {
      const res = await getRatingsByMovie(movieId)
      if (!res.success) throw new Error(res.message || "Failed to load ratings")
      return res.data
    },
    enabled: Boolean(movieId),
  })

  const m = useMutation({
    mutationFn: async () => {
      const id = movieId.trim()
      if (!id) throw new Error("MovieId is required")
      const r = Number(rating)
      const res = await addRating(id, r)
      if (!res.success) throw new Error(res.message || "Failed to add rating")
      return res.data
    },
    onSuccess: () => {
      toast.success("Rating added")
      q.refetch()
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to add rating"),
  })

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Ratings</h1>
        <p className="text-muted-foreground text-sm mt-1">
          Uses `/Rating/getRatingsByMovie` and `/Rating/add-ratings`.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Movie</CardTitle>
          <CardDescription>Enter a movieId (GUID).</CardDescription>
        </CardHeader>
        <CardContent className="grid gap-4 md:grid-cols-3">
          <div className="space-y-2 md:col-span-2">
            <Label>MovieId</Label>
            <Input
              value={movieId}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setMovieId(e.target.value)}
              placeholder="guid"
            />
          </div>
          <div className="space-y-2">
            <Label>Rating (1-5)</Label>
            <Input
              value={rating}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setRating(e.target.value)}
              placeholder="5"
            />
          </div>
          <div className="md:col-span-3">
            <Button disabled={m.isPending} onClick={() => m.mutate()}>
              {m.isPending ? "Submitting..." : "Add rating"}
            </Button>
          </div>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>Ratings</CardTitle>
          <CardDescription>{q.isLoading ? "Loading..." : ""}</CardDescription>
        </CardHeader>
        <CardContent className="space-y-2">
          {!movieId && <div className="text-sm text-muted-foreground">Enter a movieId.</div>}
          {q.isError && (
            <div className="text-destructive text-sm">{(q.error as Error).message}</div>
          )}
          {q.data?.map((r) => (
            <div key={r.ratingId} className="rounded-lg border p-3 text-sm">
              <div className="font-medium">Rating: {r.ratingScore}</div>
              <div className="text-muted-foreground">
                {new Date(r.createdAt).toLocaleString()}
              </div>
            </div>
          ))}
        </CardContent>
      </Card>
    </div>
  )
}

