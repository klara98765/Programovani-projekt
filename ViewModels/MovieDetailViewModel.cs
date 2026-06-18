using System.Collections.ObjectModel;
using Filmoteka.Models;
using Filmoteka.Repositories;

namespace Filmoteka.ViewModels;

public sealed class MovieDetailViewModel : ViewModelBase
{
    private readonly IMovieRepository _movieRepository;
    private readonly IReviewRepository _reviewRepository;
    private MainWindowViewModel? _main;
    private int _movieId;
    private int _editedReviewId;
    private Movie? _movie;
    private Review _reviewForm = new() { Rating = 5 };

    public MovieDetailViewModel(IMovieRepository movieRepository, IReviewRepository reviewRepository)
    {
        _movieRepository = movieRepository;
        _reviewRepository = reviewRepository;
        BackCommand = new ButtonCommand(_ => Back());
        EditMovieCommand = new ButtonCommand(_ => EditMovie());
        SaveReviewCommand = new ButtonCommand(_ => SaveReview());
        EditReviewCommand = new ButtonCommand(EditReviewFromCommand);
        DeleteReviewCommand = new ButtonCommand(DeleteReviewFromCommand);
    }

    public ObservableCollection<Review> Reviews { get; } = [];
    public ButtonCommand BackCommand { get; }
    public ButtonCommand EditMovieCommand { get; }
    public ButtonCommand SaveReviewCommand { get; }
    public ButtonCommand EditReviewCommand { get; }
    public ButtonCommand DeleteReviewCommand { get; }
    public Movie? Movie
    {
        get => _movie;
        set => SetProperty(ref _movie, value);
    }

    public Review ReviewForm
    {
        get => _reviewForm;
        set => SetProperty(ref _reviewForm, value);
    }

    public string ErrorMessage { get; set; } = string.Empty;

    public void Initialize(MainWindowViewModel main, int movieId)
    {
        _main = main;
        _movieId = movieId;
    }

    public void Load()
    {
        Movie = _movieRepository.GetById(_movieId);

        Reviews.Clear();
        foreach (Review review in _reviewRepository.GetByMovieId(_movieId))
        {
            Reviews.Add(review);
        }
    }

    public void Back()
    {
        if (_main == null) return;
        _main.ShowMovieList();
    }

    public void EditMovie()
    {
        if (_main == null) return;
        _main.ShowMovieForm(_movieId);
    }

    public void SaveReview()
    {
        if (string.IsNullOrWhiteSpace(ReviewForm.Author) || string.IsNullOrWhiteSpace(ReviewForm.Text))
        {
            ErrorMessage = "Vyplň autora a text recenze.";
            OnPropertyChanged(nameof(ErrorMessage));
            return;
        }

        ReviewForm.MovieId = _movieId;
        ReviewForm.ReviewedAt = DateTime.Today;

        if (_editedReviewId == 0)
        {
            _reviewRepository.Create(ReviewForm);
        }
        else
        {
            ReviewForm.Id = _editedReviewId;
            _reviewRepository.Update(ReviewForm);
        }

        ClearForm();
        Load();
    }

    public void EditReview(Review review)
    {
        _editedReviewId = review.Id;
        ReviewForm = new Review
        {
            Author = review.Author,
            Rating = review.Rating,
            Text = review.Text
        };
    }

    public void DeleteReview(Review review)
    {
        _reviewRepository.Delete(review.Id);
        Load();
    }

    private void EditReviewFromCommand(object? parameter)
    {
        Review? review = parameter as Review;
        if (review != null)
        {
            EditReview(review);
        }
    }

    private void DeleteReviewFromCommand(object? parameter)
    {
        Review? review = parameter as Review;
        if (review != null)
        {
            DeleteReview(review);
        }
    }

    private void ClearForm()
    {
        _editedReviewId = 0;
        ReviewForm = new Review { Rating = 5 };
        ErrorMessage = string.Empty;
        OnPropertyChanged(nameof(ErrorMessage));
    }
}
