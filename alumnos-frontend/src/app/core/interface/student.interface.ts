export interface Student {
  id: number;
  name: string;
  isActive: boolean;
  description: string;
}

export interface BaseResponse<T> {
  statusCode: number;
  isSuccess: boolean;
  messages: string[] | null;
  result: T;
}

export interface PaginatedResult<T> {
  items: T[];
  count: number;
  pageNumber: number;
  pageSize: number;
}

export interface StudentListRequest {
  pageNumber: number;
  pageSize: number;
}
