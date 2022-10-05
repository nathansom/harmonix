import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Job } from 'src/app/_models/job';
import { Pagination } from 'src/app/_models/pagination';
import { JobsService } from 'src/app/_services/jobs.service';

@Component({
  selector: 'app-job-saved',
  templateUrl: './job-saved.component.html',
  styleUrls: ['./job-saved.component.css']
})
export class JobSavedComponent implements OnInit {

  jobs: Partial<Job[]>;
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;


  constructor(private jobService: JobsService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadSavedJobs();
  }

  loadSavedJobs() {
    this.jobService.getSavedJobs( this.pageNumber, this.pageSize).subscribe(response => {
      this.jobs = response.result;
      this.pagination = response.pagination;
    })
  }



  removeSaveJob(job: Job) {
    this.jobService.removeSaveJob(job.id).subscribe(() => {
      this.toastr.success('You have remove this from saved jobs.');
      this.loadSavedJobs();
    })

  }

  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadSavedJobs();
  }



}