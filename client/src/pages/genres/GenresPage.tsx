import { useState, type ChangeEvent } from "react"
import { useMutation, useQuery, useQueryClient } from "@tanstack/react-query"
import { toast } from "sonner"

import { Button } from "@/components/ui/button"
import { Card, CardContent, CardDescription, CardHeader, CardTitle } from "@/components/ui/card"
import { Input } from "@/components/ui/input"
import { Label } from "@/components/ui/label"
import { addGenre, deleteGenre, getAllGenres, updateGenre } from "@/features/genres/api"

export function GenresPage() {
  const qc = useQueryClient()
  const [genreTitle, setGenreTitle] = useState("")
  const [editingId, setEditingId] = useState<string | null>(null)
  const [editingTitle, setEditingTitle] = useState("")

  const q = useQuery({
    queryKey: ["genres"],
    queryFn: async () => {
      const res = await getAllGenres()
      if (!res.success) throw new Error(res.message || "Failed to load genres")
      return res.data
    },
  })

  const m = useMutation({
    mutationFn: async () => {
      const title = genreTitle.trim()
      if (!title) throw new Error("Genre title is required")
      const res = await addGenre(title)
      if (!res.success) throw new Error(res.message || "Failed to add genre")
      return res.data
    },
    onSuccess: async () => {
      toast.success("Genre added")
      setGenreTitle("")
      await qc.invalidateQueries({ queryKey: ["genres"] })
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to add genre"),
  })

  const updateM = useMutation({
    mutationFn: async () => {
      if (!editingId) throw new Error("Missing genre id")
      const title = editingTitle.trim()
      if (!title) throw new Error("Genre title is required")
      const res = await updateGenre(editingId, title)
      if (!res.success) throw new Error(res.message || "Failed to update genre")
      return res.data
    },
    onSuccess: async () => {
      toast.success("Genre updated")
      setEditingId(null)
      setEditingTitle("")
      await qc.invalidateQueries({ queryKey: ["genres"] })
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to update genre"),
  })

  const deleteM = useMutation({
    mutationFn: async (id: string) => {
      const current = q.data?.find((x) => x.genreId === id)
      const res = await deleteGenre(id, current?.genreTitle)
      if (!res.success) throw new Error(res.message || "Failed to delete genre")
      return res.data
    },
    onSuccess: async () => {
      toast.success("Genre deleted")
      await qc.invalidateQueries({ queryKey: ["genres"] })
    },
    onError: (e: any) => toast.error(e?.message ?? "Failed to delete genre"),
  })

  return (
    <div className="space-y-4">
      <div>
        <h1 className="text-2xl font-semibold tracking-tight">Genres</h1>
        <p className="text-muted-foreground text-sm mt-1">
          Uses `GET /getAllGenre` and `POST /addGenres`.
        </p>
      </div>

      <Card>
        <CardHeader>
          <CardTitle>Add genre</CardTitle>
          <CardDescription>Create a new genre title.</CardDescription>
        </CardHeader>
        <CardContent className="flex items-end gap-3">
          <div className="flex-1 space-y-2">
            <Label>Title</Label>
            <Input
              value={genreTitle}
              onChange={(e: ChangeEvent<HTMLInputElement>) => setGenreTitle(e.target.value)}
              placeholder="Thriller"
            />
          </div>
          <Button disabled={m.isPending} onClick={() => m.mutate()}>
            {m.isPending ? "Adding..." : "Add"}
          </Button>
        </CardContent>
      </Card>

      <Card>
        <CardHeader>
          <CardTitle>All genres</CardTitle>
          <CardDescription>
            {q.isLoading ? "Loading..." : q.data ? `${q.data.length} genre(s)` : ""}
          </CardDescription>
        </CardHeader>
        <CardContent>
          {q.isError && (
            <div className="text-destructive text-sm">{(q.error as Error).message}</div>
          )}
          <div className="grid gap-2 md:grid-cols-2">
            {q.data?.map((g) => (
              <div key={g.genreId} className="rounded-lg border p-3">
                <div className="flex items-start justify-between gap-3">
                  <div className="min-w-0">
                    {editingId === g.genreId ? (
                      <div className="space-y-2">
                        <Input
                          value={editingTitle}
                          onChange={(e: ChangeEvent<HTMLInputElement>) =>
                            setEditingTitle(e.target.value)
                          }
                        />
                        <div className="flex gap-2">
                          <Button size="sm" disabled={updateM.isPending} onClick={() => updateM.mutate()}>
                            Save
                          </Button>
                          <Button
                            size="sm"
                            variant="ghost"
                            onClick={() => {
                              setEditingId(null)
                              setEditingTitle("")
                            }}
                          >
                            Cancel
                          </Button>
                        </div>
                      </div>
                    ) : (
                      <>
                        <div className="font-medium truncate">{g.genreTitle ?? "-"}</div>
                        <div className="text-xs text-muted-foreground break-all">{g.genreId}</div>
                      </>
                    )}
                  </div>

                  {editingId !== g.genreId && (
                    <div className="flex gap-2">
                      <Button
                        size="sm"
                        variant="outline"
                        onClick={() => {
                          setEditingId(g.genreId)
                          setEditingTitle(g.genreTitle ?? "")
                        }}
                      >
                        Edit
                      </Button>
                      <Button
                        size="sm"
                        variant="destructive"
                        disabled={deleteM.isPending}
                        onClick={() => deleteM.mutate(g.genreId)}
                      >
                        Delete
                      </Button>
                    </div>
                  )}
                </div>
              </div>
            ))}
          </div>
        </CardContent>
      </Card>
    </div>
  )
}

