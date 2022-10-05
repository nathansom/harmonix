import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { Member } from 'src/app/_models/member';
import { ActivatedRoute, Router } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from '@kolkov/ngx-gallery';
import { TabDirective, TabsetComponent } from 'ngx-bootstrap/tabs';
import { Message } from 'src/app/_models/message';
import { MessageService } from 'src/app/_services/message.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { AccountService } from 'src/app/_services/account.service';
import { User } from 'src/app/_models/user';
import { take } from 'rxjs/operators';
import { Job } from 'src/app/_models/job';
import { Organization } from 'src/app/_models/organization';
import { MembersService } from 'src/app/_services/members.service';
import { OrganizationsService } from 'src/app/_services/organizations.service';
import { JobsService } from 'src/app/_services/jobs.service';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-job-detail',
  templateUrl: './job-detail.component.html',
  styleUrls: ['./job-detail.component.css']
})
export class JobDetailComponent implements OnInit {
  job: Job;
  org: Organization;
  member: Member;


  constructor(private route: ActivatedRoute, private memberService: MembersService, private jobsService: JobsService, private router: Router, private orgsService: OrganizationsService, private toastr: ToastrService ) { 
  //Load member and load Organization objects from server 
  this.router.routeReuseStrategy.shouldReuseRoute = () => false;

  }

  ngOnInit(): void {

    /*this.memberService.getMemberbyId(this.job.jobPosterId).subscribe((response) => {
      this.member = response;
    });*/

    /*this.organizationService.getMemberbyId(this.job.jobPosterId).subscribe((response) => {
      this.orgs = response;
    }); 
    */
   this.loadJob();
   if(this.memberService.currMem)
   {
    this.memberService.getMember(this.memberService.currMem.username).subscribe(member =>{
      this.memberService.currMem = member;
      });
   }

  }

  loadJob(){
    this.jobsService.getJob(Number(this.route.snapshot.paramMap.get('id'))).subscribe(
      job =>{
        this.job = job;
        this.loadOrg(job.orgId);
        console.log(job.jobPosterName);
        this.loadMember(job.jobPosterName);
      }
    )
  }
  
  addLikeOrg(org: Organization) {
    this.orgsService.addLike(org.id).subscribe(() => {
      this.toastr.success('You have liked ' + org.name);
    })
  }

  loadOrg(orgId: number)
  {
    this.orgsService.getOrganization(orgId).subscribe(
      org =>{
        this.org = org;
      }
    )
  }

  loadMember(username: string)
  {
    username = username.toLowerCase();

    this.memberService.getMember(username).subscribe(
      member =>{
        this.member = member;
      }
    )
  }

  addLike(job: Job) {
    this.jobsService.saveJob(job.id).subscribe(() => {
      this.toastr.success('You have saved ' + job.title);
      this.memberService.getMember(this.memberService.currMem.username).subscribe(member =>{
        this.memberService.currMem = member;
    });
    })
  }

  jobalreadyliked()
  {
    if(this.memberService.currMem.savedJobs !== null)
    {
      if(this.memberService.currMem.savedJobs.length > 0)
      {
        var x = this.memberService.currMem.savedJobs.find((obj) => {
      return obj.title === this.job.title && obj.confirmedOrgId === this.job.confirmedOrgId && obj.jobPosterId === this.job.jobPosterId ;}
      );
      }
    }
    //console.log(this.memberService.currMem + "here");
   
    return x;
    
  }

  removeSaveJob(job: Job) {
    this.jobsService.removeSaveJob(job.id).subscribe(() => {
      this.toastr.success('You have removed '+ job.title+  ' from saved jobs.');
      this.memberService.getMember(this.memberService.currMem.username).subscribe(member =>{
        this.memberService.currMem = member;
    });
    })

  }



}
