import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { map, take } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Job } from '../_models/job';
import { JobsParams } from '../_models/jobParams';
import { User } from '../_models/user';
import { UserParams } from '../_models/userParams';
import { AccountService } from './account.service';
import { getPaginatedResult, getPaginationHeaders } from './paginationHelper';

@Injectable({
  providedIn: 'root'
})
export class JobsService {
  baseUrl = environment.apiUrl;
  jobs: Job[] = [];
  jobCache = new Map();
  jobsParams: JobsParams;


  getUserParams() {
    return this.jobsParams
  }

  setUserParams(params: JobsParams) {
    this.jobsParams = params;
  }

  resetUserParams() {
    this.jobsParams = new JobsParams();
    return this.jobsParams;
  }


  constructor(private http: HttpClient, private accountService: AccountService) {

      this.jobsParams = new JobsParams();
  }

  getJobs(jobsParams: JobsParams) {
    var response = this.jobCache.get(Object.values(jobsParams).join('-'));
    if (response) {
      return of(response);
    }
  
    let params = getPaginationHeaders(jobsParams.pageNumber, jobsParams.pageSize);

    var regex = /^[a-zA-Z0-9 ]+$/g; //checks if the string is just empty spaces could be improved
    if(regex.test(jobsParams.title) && jobsParams.title)
    {
      jobsParams.title = jobsParams.title.replace(" ", "%20");
    }

    params = params.append('title', jobsParams.title);
    params = params.append('jobType', jobsParams.jobType);
    params = params.append('selfPost', jobsParams.selfPost);
    params = params.append('orderBy', jobsParams.orderBy);
    console.log(jobsParams)

    return getPaginatedResult<Job[]>(this.baseUrl + 'jobs', params, this.http)
      .pipe(map(response => {
        this.jobCache.set(Object.values(jobsParams).join('-'), response);
        return response;
      }));
  }

  getJob(id: number) {
    const job = [...this.jobCache.values()]
      .reduce((arr, elem) => arr.concat(elem.result), [])
      .find((job: Job) => job.id === id);

    if (job) {
      return of(job);
    }
    return this.http.get<Job>(this.baseUrl + 'jobs/' + id);
  }

  getJobsByPosterId(id: number, jobsParams: JobsParams) {
    let params = getPaginationHeaders(jobsParams.pageNumber, jobsParams.pageSize);


    return getPaginatedResult<Job[]>(this.baseUrl+'jobs/poster/'+id, params, this.http)
            .pipe(map(response => {
                this.jobCache.set(Object.values(jobsParams).join('-'), response);
                return response;
            }));
  }

  getJobsByTitle(title: string, jobsParams: JobsParams) {
    let params = getPaginationHeaders(jobsParams.pageNumber, jobsParams.pageSize);


    return getPaginatedResult<Job[]>(this.baseUrl + 'jobs/title/' + title, params, this.http)
            .pipe(map(response => {
                //this.jobCache.set(Object.values(jobsParams).join('-'), response);
                return response;
            }));
  }

  updateJob(job: Job, id: number) {
    return this.http.put(this.baseUrl + 'jobs/' + id, job)
    .pipe(
      map(() => {
        const index = this.jobs.indexOf(job);
        this.jobs[index] = job;
      })
    );
  }

  saveJob(id: number) {
    return this.http.post(this.baseUrl + 'jobsave/' + id, {});
  }

  getSavedJobs( pageNumber, pageSize) {
    let params = getPaginationHeaders(pageNumber, pageSize);
    
    return getPaginatedResult<Partial<Job[]>>(this.baseUrl + 'jobsave', params, this.http);
  }

  registerJob(model: any) {

    //console.log(model);
    
    return this.http.post(this.baseUrl + 'jobs/add', model);
  }

  removeSaveJob(id: number) {
    return this.http.delete(this.baseUrl + 'jobsave/delete-savedJob/' + id, {});
};

}
