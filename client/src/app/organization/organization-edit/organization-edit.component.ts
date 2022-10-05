import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { JobsParams } from 'src/app/_models/jobParams';
import { Member } from 'src/app/_models/member';
import { User } from 'src/app/_models/user';
import { AccountService } from 'src/app/_services/account.service';
import { JobsService } from 'src/app/_services/jobs.service';
import { MembersService } from 'src/app/_services/members.service';
import { OrganizationsService } from 'src/app/_services/organizations.service';
import { take } from 'rxjs/operators';
import { OrgParams } from 'src/app/_models/orgParams';
import { Organization } from 'src/app/_models/organization';
import { Pagination } from 'src/app/_models/pagination';

@Component({
  selector: 'app-organization-edit',
  templateUrl: './organization-edit.component.html',
  styleUrls: ['./organization-edit.component.css']
})
export class OrganizationEditComponent implements OnInit {
  organization: any;
  @ViewChild('editForm') editForm: NgForm;
  user: User;
  member: Member;
  orgParams: OrgParams;
  userOrgs: Organization[];
  pagination: Pagination;

  constructor(private organizationService: OrganizationsService, private toastr: ToastrService, 
    private accountService: AccountService, private memberService: MembersService) {
    this.orgParams = this.organizationService.getOrgParams();
    this.orgParams.pageSize = 3;
    this.accountService.currentUser$.pipe(take(1)).subscribe(user => this.user = user);
    this.memberService.getMember(this.user.username).subscribe((m) => {
      this.loadOrganizationsByUserId(m.id);
      
      this.member = m;
    });
   }

  ngOnInit(): void {
    
  }

  loadOrganization(org: any){
    this.organization = org;
  }

  loadOrganizationsByUserId(id: number){
    this.organizationService.setOrgParams(this.orgParams);
    this.organizationService.getOrgByPosterId(id, this.orgParams).subscribe((response) => {
      if(response?.result?.length>0){
        this.loadOrganization(response.result[0]);
      }
      console.log(response.result);
      this.userOrgs = response.result;
      this.pagination = response.pagination;
    });
  }

  load(id: number){
    console.log("log id "+id);
    this.organizationService.getOrganization(id).subscribe(
      org => {
        this.organization = org;
        // this.galleryImages = this.getImages(org);
        this.editForm.reset(org);
      }
    )

  }


  getDummyImages(){
    var images = [];
    images.push({
      isMain: true,
      url: 'https://i.picsum.photos/id/988/200/200.jpg?hmac=-lwK-i6PssD9WlUeVPDIhOxDVxlzJKeM4MgEx_fIqJg'
    });
    images.push({
      isMain: false,
      url: 'https://i.picsum.photos/id/367/200/200.jpg?hmac=6NmiWxiENMBIeAXEfu9fN20uigiBudgYzqHfz-eXZYk'
    });
    images.push({
      isMain: false,
      url: 'https://i.picsum.photos/id/214/200/200.jpg?hmac=hcznBngs7e7PmNwXcM4UioAhb1oOUpfGDzBM-qSgpp4'
    });
    return images;
  }

  update(){
    this.organizationService.updateOrganization(this.organization.id, this.editForm.value).subscribe(() => {
      this.toastr.success('Profile updated successfully');
      this.organization = this.editForm.value;
      this.editForm.reset(this.organization);
    })
  }

  resetFilters() {
    this.orgParams = this.organizationService.resetOrgParams();
    this.loadOrganizationsByUserId(this.member.id);
  }

  pageChanged(event: any) {
    this.orgParams.pageNumber = event.page;
    this.organizationService.setOrgParams(this.orgParams);
    this.loadOrganizationsByUserId(this.member.id);
  }

}
