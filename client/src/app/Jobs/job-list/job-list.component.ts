import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { GENDER_LIST, JOB_TYPE } from 'src/app/util/constants';
import { Job } from 'src/app/_models/job';
import { JobsParams } from 'src/app/_models/jobParams';
import { JobsService } from 'src/app/_services/jobs.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-job-list',
  templateUrl: './job-list.component.html',
  styleUrls: ['./job-list.component.css']
})
export class JobListComponent implements OnInit {
  jobs: Job [];
  user: User;
  member: Member;
  pagination: Pagination;
  postedByUser: Boolean;
  jobParams: JobsParams;
  title: string;
  postedloded: boolean = false;
  jobTypeList = JOB_TYPE;

  constructor(private memberService: MembersService, private jobsService: JobsService, private accountService: AccountService) {
    this.jobParams = this.jobsService.getUserParams();
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.memberService.getMember(this.user.username).subscribe((m) => {
      console.log(m);
      this.member = m;
    });
  }

  ngOnInit(): void {
    this.loadJobs();
  }

  loadJobs() {
    this.jobsService.setUserParams(this.jobParams);
    this.jobsService.getJobs(this.jobParams).subscribe((response) => {
      this.jobs = response.result;
      console.log(response.result);
      this.pagination = response.pagination;
    });

    
  }


  btnClick()
  {
    if(this.title != null)
    {
      if(this.title != "")
      {
        this.title = this.title.replace(" ", "%20");
        this.loadJobsByTitle(this.title);
      }
      else
      {
        this.resetFilters();
      }
    }  
  }

  loadJobsByUserId(id: number)
  {
    if( this.postedloded == false)
    {    
      this.jobsService.setUserParams(this.jobParams);
      this.jobsService.getJobsByPosterId(id, this.jobParams).subscribe((response) => {
        this.jobs = response.result;
        this.pagination = response.pagination;
      });
      this.postedloded = true;
    }

    else{
      this.resetFilters();
      
      this.postedloded = false;
    }


  }

  loadJobsByTitle(title: string)
  {
    this.jobsService.setUserParams(this.jobParams);
    this.jobsService.getJobsByTitle(title, this.jobParams).subscribe((response) => {
      this.jobs = response.result;
      this.pagination = response.pagination;
    });

  }

  resetFilters() {
    this.jobParams = this.jobsService.resetUserParams();
    this.loadJobs();
  }

  pageChanged(event: any) {
    this.jobParams.pageNumber = event.page;
    this.jobsService.setUserParams(this.jobParams);
    this.loadJobs();
  }
}
