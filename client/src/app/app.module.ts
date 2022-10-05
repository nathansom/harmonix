import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NavComponent } from './nav/nav.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HomeComponent } from './home/home.component';
import { RegisterComponent } from './register/register.component';
import { MemberListComponent } from './members/member-list/member-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessagesComponent } from './messages/messages.component';
import { SharedModule } from './_modules/shared.module';
import { TestErrorsComponent } from './errors/test-errors/test-errors.component';
import { ErrorInterceptor } from './_interceptors/error.interceptor';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { ServerErrorComponent } from './errors/server-error/server-error.component';
import { MemberCardComponent } from './members/member-card/member-card.component';
import { JwtInterceptor } from './_interceptors/jwt.interceptor';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { LoadingInterceptor } from './_interceptors/loading.interceptor';
import { PhotoEditorComponent } from './_interceptors/photo-editor/photo-editor.component';
import { TextInputComponent } from './_forms/text-input/text-input.component';
import { DateInputsComponent } from './_forms/date-input/date-input.component';
import { MemberMessagesComponent } from './members/member-messages/member-messages.component';
import { AdminPanelComponent } from './admin/admin-panel/admin-panel.component';
import { HasRoleDirective } from './_directives/has-role.directive';
import { UserManagementComponent } from './admin/user-management/user-management.component';
import { PhotoManagementComponent } from './admin/photo-management/photo-management.component';
import { RolesModalComponent } from './modals/roles-modal/roles-modal.component';
import { JobCardComponent } from './Jobs/job-card/job-card.component';
import { JobDetailComponent } from './Jobs/job-detail/job-detail.component';
import { JobListComponent } from './Jobs/job-list/job-list.component';
import { JobEditComponent } from './Jobs/job-edit/job-edit.component';
import { OrganizationEditComponent } from './organization/organization-edit/organization-edit.component';
import { OrganizationCardComponent } from './organization/organization-card/organization-card.component';
import { OrganizationDetailComponent } from './organization/organization-detail/organization-detail.component';
import { OrganizationListComponent } from './organization/organization-list/organization-list.component';
import { JobRegisterComponent } from './Jobs/job-register/job-register.component';
import { OrganizationRegisterComponent } from './organization/organization-register/organization-register.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { MultiselectComponent } from './_forms/multiselect/multiselect.component';
import { JobSavedComponent } from './Jobs/job-saved/job-saved.component';
import { OrgLikedComponent } from './organization/org-liked/org-liked.component';
import { LearnMoreComponent } from './learn-more/learn-more.component';
import { PricingComponent } from './pricing/pricing.component';

@NgModule({
  declarations: [
    AppComponent,
    NavComponent,
    HomeComponent,
    RegisterComponent,
    MemberListComponent,
    MemberDetailComponent,
    ListsComponent,
    MessagesComponent,
    TestErrorsComponent,
    NotFoundComponent,
    ServerErrorComponent,
    MemberCardComponent,
    MemberEditComponent,
    PhotoEditorComponent,
    TextInputComponent,
    DateInputsComponent,
    MemberMessagesComponent,
    AdminPanelComponent,
    HasRoleDirective,
    UserManagementComponent,
    PhotoManagementComponent,
    RolesModalComponent,
    JobCardComponent,
    JobDetailComponent,
    JobListComponent,
    JobEditComponent,
    OrganizationEditComponent,
    OrganizationCardComponent,
    OrganizationDetailComponent,
    OrganizationListComponent,
    JobRegisterComponent,
    OrganizationRegisterComponent,
    MultiselectComponent,
    JobSavedComponent,
    OrgLikedComponent,
    LearnMoreComponent,
    PricingComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    HttpClientModule,
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    SharedModule,
    NgMultiSelectDropDownModule.forRoot()
  ],
  providers: [
    { provide: HTTP_INTERCEPTORS, useClass: ErrorInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: JwtInterceptor, multi: true },
    { provide: HTTP_INTERCEPTORS, useClass: LoadingInterceptor, multi: true }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
