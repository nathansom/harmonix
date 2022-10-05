import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { HomeComponent } from './home/home.component';
import { JobDetailComponent } from './Jobs/job-detail/job-detail.component';
import { JobEditComponent } from './Jobs/job-edit/job-edit.component';
import { JobListComponent } from './Jobs/job-list/job-list.component';
import { JobRegisterComponent } from './Jobs/job-register/job-register.component';
import { JobSavedComponent } from './Jobs/job-saved/job-saved.component';
import { LearnMoreComponent } from './learn-more/learn-more.component';
import { ListsComponent } from './lists/lists.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MessagesComponent } from './messages/messages.component';
import { OrgLikedComponent } from './organization/org-liked/org-liked.component';
import { OrganizationDetailComponent } from './organization/organization-detail/organization-detail.component';
import { OrganizationEditComponent } from './organization/organization-edit/organization-edit.component';
import { OrganizationListComponent } from './organization/organization-list/organization-list.component';
import { OrganizationRegisterComponent } from './organization/organization-register/organization-register.component';
import { AdminGuard } from './_guards/admin.guard';
import { AuthGuard } from './_guards/auth.guard';
import { PreventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { MemberDetailedResolver } from './_resolvers/member-detailed.resolver';


const routes: Routes = [
  {path: '', component: HomeComponent},
  {
    path: '', 
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      {path: 'members', component: MemberListComponent},
      {path: 'members/:username', component: MemberDetailComponent, resolve: {member: MemberDetailedResolver}},
      {path: 'member/edit', component: MemberEditComponent, canDeactivate: [PreventUnsavedChangesGuard]},
      {path: 'member/job_edit', component: JobEditComponent},
      {path: 'jobs/:id', component: JobDetailComponent},
      {path: 'member/org_edit', component: OrganizationEditComponent},
      {path: 'organizations/:id', component: OrganizationDetailComponent},
      {path: 'lists', component: ListsComponent},
      {path: 'messages', component: MessagesComponent},
      {path: 'admin', component: AdminPanelComponent, canActivate: [AdminGuard]},
      {path: 'jobs', component: JobListComponent},
      {path: 'job/new_post', component: JobRegisterComponent},
      {path: 'jobs_saved', component: JobSavedComponent},
      {path: 'orgs_saved', component: OrgLikedComponent},
      {path: 'organizations', component: OrganizationListComponent},
      {path: 'organization/register', component: OrganizationRegisterComponent}
    ]
  },
  {path: 'errors', component: TestErrorsComponent},
  {path: 'learn-more', component: LearnMoreComponent},
  {path: 'not-found', component: NotFoundComponent},
  {path: 'server-error', component: ServerErrorComponent},
  {path: '**', component: NotFoundComponent, pathMatch: 'full'},
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
