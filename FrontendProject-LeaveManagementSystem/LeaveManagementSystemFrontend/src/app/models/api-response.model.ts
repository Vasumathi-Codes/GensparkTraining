export interface ApiResponse<T> {
  data: T;
  pagination?: Pagination;
}

export interface Pagination {
  totalRecords: number;
  page: number;
  pageSize: number;
  totalPages: number;
}
