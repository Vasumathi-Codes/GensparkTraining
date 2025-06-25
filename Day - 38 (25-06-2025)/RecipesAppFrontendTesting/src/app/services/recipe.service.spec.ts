import { TestBed } from '@angular/core/testing';
import { RecipeService } from './recipe.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';

describe('RecipeService', () => {
  let service: RecipeService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],
      providers: [RecipeService]
    });

    service = TestBed.inject(RecipeService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  it('should fetch recipes from API', () => {
    const mockResponse = {
      recipes: [
        { name: 'Pizza', cuisine: 'Italian' },
        { name: 'Pasta', cuisine: 'Italian' }
      ]
    };

    service.getAllRecipes().subscribe((response) => {
      console.log(response.recipes);
      expect(response).toEqual(mockResponse);
      expect(response.recipes.length).toBe(2);
    });

    const req = httpMock.expectOne('https://dummyjson.com/recipes');
    expect(req.request.method).toBe('GET');
    req.flush(mockResponse); 
  });

  afterEach(() => {
    httpMock.verify(); 
  });
});
