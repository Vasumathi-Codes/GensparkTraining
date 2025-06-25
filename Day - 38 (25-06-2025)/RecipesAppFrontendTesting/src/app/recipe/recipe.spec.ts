import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipe } from './recipe';
import { RecipeModel } from '../models/recipe.model';
import { RecipeService } from '../services/recipe.service';
import { of } from 'rxjs';

describe('Recipe Component (Standalone)', () => {
  let fixture: ComponentFixture<Recipe>;
  let component: Recipe;

  const mockRecipe: RecipeModel = {
    id: 1,
    name: 'Pizza',
    cuisine: 'Italian',
    cookTimeMinutes: 30,
    ingredients: 'Flour, Tomato, Cheese',
    image: 'pizza.jpg'
  };

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [Recipe],
      providers: [
        {
          provide: RecipeService,
          useValue: {
            getAllRecipes: () => of([]) 
          }
        }
      ]
    }).compileComponents();

    fixture = TestBed.createComponent(Recipe);
    component = fixture.componentInstance;
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should render recipe input data', () => {
    component.recipe = mockRecipe;
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;

    expect(compiled.querySelector('.recipe-title')?.textContent).toContain('Pizza');
    expect(compiled.querySelector('.recipe-detail')?.textContent).toContain('Italian');
    expect(compiled.querySelector('img')?.getAttribute('src')).toBe('pizza.jpg');
    expect(compiled.querySelector('.recipe-ingredients')?.textContent).toContain('Flour, Tomato, Cheese');
  });
});
