import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TopicResponseDto, TopicRequestDto } from '../models/api.models';

@Injectable({
  providedIn: 'root'
})
export class TopicService {
  private apiUrl = 'http://localhost:8005/api/v1/topics';

  constructor(private http: HttpClient) {}

  getTopics(): Observable<TopicResponseDto[]> {
    return this.http.get<TopicResponseDto[]>(this.apiUrl);
  }

  createTopic(topic: TopicRequestDto): Observable<TopicResponseDto> {
    return this.http.post<TopicResponseDto>(this.apiUrl, topic);
  }
}