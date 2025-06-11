import { Component, OnInit, Signal, inject, signal } from '@angular/core';
import { RecipeService } from '../services/recipe.service';
import { RecipeModel } from '../models/recipe.model';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-recipe',
  templateUrl: './recipe.html',
  imports: [CommonModule],
  styleUrls: ['./recipe.css'],
  standalone: true
})
export class RecipeComponent implements OnInit {
  private recipeService = inject(RecipeService);

  // Signals to hold data and error info
  recipes = signal<RecipeModel[]>([]);
  errorMessage = signal<string>(''); 

  constructor() {}

  ngOnInit(): void {
    this.loadRecipes();
  }

  loadRecipes() {
    this.recipeService.getRecipes().subscribe({
      next: (data) => {
        this.recipes.set(data);
        this.errorMessage.set('');
      },
      error: (error) => {
        console.error('Error fetching recipes:', error);
        this.errorMessage.set('Failed to load recipes. Please try again later.');
      }
    });
  }
}
