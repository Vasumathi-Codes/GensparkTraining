import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { RecipeModel } from '../models/recipe.model';
import { Observable, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class RecipeService {
  private apiUrl = 'https://dummyjson.com/recipes';

  constructor(private http: HttpClient) {}

  getRecipes(): Observable<RecipeModel[]> {
    return this.http.get<any>(this.apiUrl).pipe(
      map(response => response.recipes as RecipeModel[])
    );
  }
}
