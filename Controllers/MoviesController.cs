using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesApp.Models;

public class MoviesController : Controller
{
    //DRY => DON'T REPEAT YOURSELF
    private readonly ApplicationDbContext _context;

    public MoviesController(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET: MOVIES
    public async Task<IActionResult> Index(string movieGenre, string searchString)    
    {
        if (_context.Movies is null) return Problem("Entity set null");

        var genres = _context.Movies.Select(m => m.Genre).Distinct().Order();
        var movies = _context.Movies.Select(m => m);

        if (!string.IsNullOrEmpty(searchString))
            movies = movies.Where(m => m.Title!.ToUpper().StartsWith(searchString.ToUpper()));

        if (!string.IsNullOrEmpty(movieGenre))
            movies = movies.Where(m => m.Genre == movieGenre);


        var movieGenreVM = new MovieGenreViewModel
        {
            Genres = new SelectList(await genres.ToListAsync()),
            Movies = await movies.ToListAsync(),
        };

        return View(movieGenreVM);
    }

    // GET: MOVIES/Details/{id}
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null) return NotFound();

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);

        if (movie == null) return NotFound();

        return View(movie);
    }

    // GET: MOVIES/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: MOVIES/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (ModelState.IsValid)
        {
            _context.Add(movie);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Edit/{id}
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies.FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Edit/{id}
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int? id, [Bind("Id,Title,ReleaseDate,Genre,Price,Rating")] Movie movie)
    {
        if (id != movie.Id)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _context.Update(movie);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!MovieExists(movie.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction(nameof(Index));
        }
        return View(movie);
    }

    // GET: MOVIES/Delete/{id}
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var movie = await _context.Movies
            .FirstOrDefaultAsync(m => m.Id == id);
        if (movie == null)
        {
            return NotFound();
        }

        return View(movie);
    }

    // POST: MOVIES/Delete/{id}
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int? id)
    {
        var movie = await _context.Movies.FindAsync(id);
        if (movie != null)
        {
            _context.Movies.Remove(movie);
        }

        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool MovieExists(int? id) => _context.Movies.Any(e => e.Id == id);
}
