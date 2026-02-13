import { BasePagedQuery } from '../../core/models/paging/base-paged-query';
import { PageResult } from '../../core/models/paging/page-result';

// === ENUMS ===

/**
 * Tip fakture enum
 * Corresponds to: FakturaTip.cs
 */
export enum FakturaTip {
  /** Ulazna faktura - unos robe / povećanje zaliha */
  Ulazna = 1,
  /** Izlazna faktura - iznos robe / smanjenje zaliha */
  Izlazna = 2,
}

export interface StavkaFakture {
  proizvod: string;
  kolicina: number;
  kategorijaId: number;
}

export interface CreateFakturaCommand {
  brojRacuna: string;
  tip: FakturaTip;
  napomena: string;
  items: StavkaFakture[];
}

// === QUERIES (READ) ===

/**
 * Response item for GET /Fakture
 * Corresponds to: ListFaktureQueryDto.cs
 */
export interface ListFaktureQueryDto {
  id: number;
  brojRacuna: string;
  tip: FakturaTip;
  datumKreiranja: string; // ISO date string
  brojStavki: number;
}

export class ListFaktureRequest extends BasePagedQuery {}

/**
 * Paged response for GET /Fakture
 */
export type ListFaktureResponse = PageResult<ListFaktureQueryDto>;
