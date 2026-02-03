export interface IApiResponse<T> {
  data: T;
  isSuccess: boolean;
  message: string;
  errorType?: string;
}