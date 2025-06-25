import { ComponentFixture, TestBed } from '@angular/core/testing';
import { Recipes } from './recipes';
import { RecipeService } from '../services/recipe.service';
import { of, throwError } from 'rxjs';
import { RecipeModel } from '../models/recipe.model';
import { Component } from '@angular/core';

// Stub for <app-recipe> used inside Recipes component
@Component({
  selector: 'app-recipe',
  standalone: true,
  template: ''
})
class MockRecipeComponent {}

describe('Recipes Component (Standalone)', () => {
  let fixture: ComponentFixture<Recipes>;
  let component: Recipes;
  let recipeServiceSpy: jasmine.SpyObj<RecipeService>;

  const mockRecipes: RecipeModel[] = [
    {
      id: 1,
      name: 'Pizza',
      cuisine: 'Italian',
      cookTimeMinutes: 30,
      ingredients: 'Flour, Tomato, Cheese',
      image: 'pizza.jpg'
    },
    {
      id: 2,
      name: 'Sushi',
      cuisine: 'Japanese',
      cookTimeMinutes: 50,
      ingredients: 'Rice, Fish',
      image: 'sushi.jpg'
    }
  ];

  beforeEach(async () => {
    const spy = jasmine.createSpyObj('RecipeService', ['getAllRecipes']);

    await TestBed.configureTestingModule({
      imports: [Recipes, MockRecipeComponent],
      providers: [{ provide: RecipeService, useValue: spy }]
    }).compileComponents();

    recipeServiceSpy = TestBed.inject(RecipeService) as jasmine.SpyObj<RecipeService>;

    recipeServiceSpy.getAllRecipes.and.returnValue(of({ recipes: [] }));

    fixture = TestBed.createComponent(Recipes);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create the component', () => {
    expect(component).toBeTruthy();
  });

  it('should load and render recipes when service returns data', () => {
    recipeServiceSpy.getAllRecipes.and.returnValue(of({ recipes: mockRecipes }));
    
    component.loadRecipes();
    fixture.detectChanges();

    expect(component.recipes()?.length).toBe(2);
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.querySelectorAll('app-recipe').length).toBe(2);
  });


  it('should show fallback UI when no recipes are returned', () => {
    component.recipes.set([]);
    fixture.detectChanges();

    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No Recipes Available');
  });


  it('should set empty list and handle error when service fails', () => {
    recipeServiceSpy.getAllRecipes.and.returnValue(throwError(() => new Error('fail')));
    
    component.loadRecipes();
    fixture.detectChanges();

    expect(component.recipes()?.length).toBe(0);
    const compiled = fixture.nativeElement as HTMLElement;
    expect(compiled.textContent).toContain('No Recipes Available');
  });
});
