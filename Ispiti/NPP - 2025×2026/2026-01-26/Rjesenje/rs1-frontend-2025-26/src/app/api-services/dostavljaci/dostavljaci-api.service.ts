import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../../environments/environment';
import { buildHttpParams } from '../../core/models/build-http-params';
import {
  CreateDostavljacCommand,
  GetDostavljacByIdQueryDto,
  ListDostavljaciQuery,
  ListDostavljaciResponse,
  UpdateDostavljacCommand,
} from './dostavljaci-api.models';

@Injectable({
  providedIn: 'root',
})
export class DostavljacApiService {
  private readonly baseUrl = `${environment.apiUrl}/Dostavljaci`;
  private http = inject(HttpClient);

  /**
   * GET /Dostavljac
   * List Dostavljac with optional query parameters.
   */
  list(request?: ListDostavljaciQuery): Observable<ListDostavljaciResponse> {
    const params = request ? buildHttpParams(request as any) : undefined;

    return this.http.get<ListDostavljaciResponse>(this.baseUrl, {
      params,
    });
  }

  /**
   * GET /Dostavljac/{id}
   * Get a single Dostavljac by ID.
   */
  getById(id: number): Observable<GetDostavljacByIdQueryDto> {
    return this.http.get<GetDostavljacByIdQueryDto>(`${this.baseUrl}/${id}`);
  }

  /**
   * POST /Dostavljac
   * Create a new Dostavljac.
   * @returns ID of the newly created Dostavljac
   */
  create(payload: CreateDostavljacCommand): Observable<number> {
    return this.http.post<number>(this.baseUrl, payload);
  }

  /**
   * PUT /Dostavljac/{id}
   * Update an existing Dostavljac.
   */
  update(id: number, payload: UpdateDostavljacCommand): Observable<void> {
    return this.http.put<void>(`${this.baseUrl}/${id}`, payload);
  }

  /**
   * DELETE /Dostavljac/{id}
   * Delete a Dostavljac.
   */
  delete(id: number): Observable<void> {
    return this.http.delete<void>(`${this.baseUrl}/${id}`);
  }
}
