import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Job } from 'src/app/_models/Job';
import { Organization } from 'src/app/_models/organization';
import { orgLike } from 'src/app/_models/orgLike';
import { Pagination } from 'src/app/_models/pagination';
import { JobsService } from 'src/app/_services/jobs.service';
import { OrganizationsService } from 'src/app/_services/organizations.service';

@Component({
  selector: 'org-liked-saved',
  templateUrl: './org-liked.component.html',
  styleUrls: ['./org-liked.component.css']
})
export class OrgLikedComponent implements OnInit {

  orgs: Partial<Organization[]> = [];
  pageNumber = 1;
  pageSize = 5;
  pagination: Pagination;


  constructor(private orgsService: OrganizationsService, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.loadSavedOrgs();
  }

  loadSavedOrgs() {
    this.orgs = [];
    this.orgsService.getLikes( this.pageNumber, this.pageSize).subscribe(response => {
      this.orgs = response.result;
      this.pagination = response.pagination;
    })
  }




  /*removeSaveJob(job: Job) {
    this.jobService.removeSaveJob(job.id).subscribe(() => {
      this.toastr.success('You have remove this from saved jobs.');
      this.loadSavedJobs();
    })


  }*/



  pageChanged(event: any) {
    this.pageNumber = event.page;
    this.loadSavedOrgs();
  }


}