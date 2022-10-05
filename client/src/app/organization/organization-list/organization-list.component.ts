import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { take } from 'rxjs/operators';
import { Member } from 'src/app/_models/member';
import { Pagination } from 'src/app/_models/pagination';
import { User } from 'src/app/_models/user';
import { UserParams } from 'src/app/_models/userParams';
import { AccountService } from 'src/app/_services/account.service';
import { MembersService } from 'src/app/_services/members.service';
import { GENDER_LIST } from 'src/app/util/constants';
import { Job } from 'src/app/_models/job';
import { JobsParams } from 'src/app/_models/jobParams';
import { JobsService } from 'src/app/_services/jobs.service';
import { Router } from '@angular/router';
import { Organization } from 'src/app/_models/organization';
import { OrgParams } from 'src/app/_models/orgParams';
import { OrganizationsService } from 'src/app/_services/organizations.service';
@Component({
  selector: 'app-organization-list',
  templateUrl: './organization-list.component.html',
  styleUrls: ['./organization-list.component.css']
})
export class OrganizationListComponent implements OnInit {
  orgs: Organization [];
  user: User;
  member: Member;
  pagination: Pagination;
  postedByUser: Boolean;
  orgParams: OrgParams;
  postedloded: boolean = false;

  genderList = GENDER_LIST;

  constructor(private memberService: MembersService, private orgsService: OrganizationsService, private accountService: AccountService) {
    this.orgParams = this.orgsService.getOrgParams();
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.memberService.getMember(this.user.username).subscribe(member => {
      this.member = member;
    });


  }

  ngOnInit(): void {
    this.loadMembers();
  }

  loadMembers() {
    this.orgsService.setOrgParams(this.orgParams);
    this.orgParams.pageSize = 5;

    this.orgsService.getOrganizations(this.orgParams).subscribe((response) => {
      this.orgs = response.result;
      this.pagination = response.pagination;
    });
  }

  btnClick()
  {

  }

  loadOrganizationsByUserId(id: number){


    if( this.postedloded == false)
    {    
      this.orgsService.setOrgParams(this.orgParams);
      this.orgsService.getOrgByPosterId(id, this.orgParams).subscribe((response) => {
        this.orgs = response.result;
        this.pagination = response.pagination;
      });
      this.postedloded = true;
    }

    else{
      this.resetFilters();
      this.postedloded = false;
    }

  }




  resetFilters() {
    this.orgParams = this.orgsService.resetOrgParams();
    this.loadMembers();
  }

  pageChanged(event: any) {
    this.orgParams.pageNumber = event.page;
    this.orgsService.setOrgParams(this.orgParams);
    this.loadMembers();
  }
}
