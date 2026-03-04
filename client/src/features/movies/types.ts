export type Film = {
  movieId: string
  movieTitle: string
  releaseYear: string
  runTime: string
  posterUrl?: string | null
  movieDescription?: string | null
  averageRatings?: number
  totalRatings?: number
  movieGenre?: { genreId: string; genreTitle?: string | null }[]
  movieRatings?: {
    ratingId: string
    ratingScore: number
    createdAt: string
    movieId: string
    userId: string
  }[]
}

export type FilmFilter = {
  genreId?: string
  releaseYear?: string
  minRating?: number
  maxRating?: number
}

