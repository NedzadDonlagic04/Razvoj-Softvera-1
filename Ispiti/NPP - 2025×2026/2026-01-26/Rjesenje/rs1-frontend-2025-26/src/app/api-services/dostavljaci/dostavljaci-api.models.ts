import { PageResult } from '../../core/models/paging/page-result';
import { BasePagedQuery } from '../../core/models/paging/base-paged-query';

export enum DostavljacType {
  Ekstern = 1,
  Intern,
  Freelancer,
}

// === QUERIES (READ) ===

/**
 * Query parameters for GET /Dostavljacs
 * Corresponds to: ListDostavljaciQuery.cs
 */
export class ListDostavljaciQuery extends BasePagedQuery {
  search?: string | null;
  // Future filters: categoryId?, isEnabled?, priceMin?, priceMax?
}

/**
 * Response item for GET /Dostavljacs
 * Corresponds to: ListDostavljaciQueryDto.cs
 */
export interface ListDostavljaciQueryDto {
  id: number;
  name: string;
  type: DostavljacType;
  code: string;
  isActive: boolean;
}

/**
 * Response for GET /Dostavljacs/{id}
 * Corresponds to: GetDostavljacByIdQueryDto.cs
 */
export interface GetDostavljacByIdQueryDto {
  id: number;
  name: string;
  type: DostavljacType;
  code: string;
  isActive: boolean;
}

/**
 * Paged response for GET /Dostavljacs
 */
export type ListDostavljaciResponse = PageResult<ListDostavljaciQueryDto>;

// === COMMANDS (WRITE) ===

/**
 * Command for POST /Dostavljacs
 * Corresponds to: CreateDostavljacCommand.cs
 */
export interface CreateDostavljacCommand {
  name: string;
  type: DostavljacType;
  code: string;
  isActive: boolean;
}

/**
 * Command for PUT /Dostavljacs/{id}
 * Corresponds to: UpdateDostavljacCommand.cs
 */
export interface UpdateDostavljacCommand {
  name: string;
  type: DostavljacType;
  code: string;
  isActive: boolean;
}
