export interface IApiResponse<T> {
  data: T;
  isSuccess: boolean;
  message: string;
  errorType?: string;
}

export interface IPagination {
  currentPage: number;
  totalPages: number;
  pageSize: number;
  totalCount: number;
  hasPrevious: boolean;
  hasNext: boolean;
}

export interface IApiResponsePg<T> {
  data: T;
  isSuccess: boolean;
  error: string;
  errorType: string;
  pagination?: IPagination; 
}