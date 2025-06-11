export class RecipeModel {
  constructor(
    public id: number = 0,
    public name: string = '',
    public cuisine: string = '',
    public difficulty: string = '',
    public ingredients: string[] = [],
    public cookTimeMinutes: number = 0,
    public image: string = ''
  ) {}
}
