import { Component, Input, OnInit } from '@angular/core';
import { FileUploader } from 'ng2-file-upload';
import { Member } from 'src/app/_models/member';
import { AccountService } from 'src/app/_services/account.service';
import { environment } from 'src/environments/environment';
import { take } from 'rxjs/operators';
import { User } from 'src/app/_models/user';
import { MembersService } from 'src/app/_services/members.service';
import { Photo } from 'src/app/_models/photo';
import { OrganizationsService } from 'src/app/_services/organizations.service';

@Component({
  selector: 'app-photo-editor',
  templateUrl: './photo-editor.component.html',
  styleUrls: ['./photo-editor.component.css']
})
export class PhotoEditorComponent implements OnInit {
  @Input() member: any;
  @Input() uploadType = "member"; //flag variable, please pass this flag when using this component and change your endpoint accordingly
  uploader: FileUploader;
  hasBaseDropZoneOver = false;
  baseUrl = environment.apiUrl;
  user: User;

  constructor(private accountService: AccountService, private membersService: MembersService, private orgService: OrganizationsService) {
    this.accountService.currentUser$.pipe(take(1)).subscribe(user =>
      this.user = user);
   }

  ngOnInit(): void {
    this.initializeUpdloader();
  }

  fileOverBase(e: any) {
    this.hasBaseDropZoneOver = e;
  }

  setMainPhoto(photo: Photo) {
    if(this.uploadType === "org"){
      this.orgService.setMainPhoto(photo.id, this.member.id).subscribe(() => {
        this.member.photoUrl = photo.url;
        this.member.photos.forEach(p=> {
          if(p.isMain) p.isMain = false;
          if(p.id == photo.id) p.isMain = true;
        });
      })

    }else if(this.uploadType === "member"){
      this.membersService.setMainPhoto(photo.id).subscribe(() => {
        this.user.photoUrl = photo.url;
        this.accountService.setCurrentUser(this.user);
        this.member.photoUrl = photo.url;
        this.member.photos.forEach(p => {
          if (p.isMain) p.isMain = false;
          if (p.id === photo.id) p.isMain = true;
        });
      })
    }
    
  }

  deletePhoto(photoId: number) {
    if(this.uploadType === "org"){
      this.orgService.deletePhoto(photoId).subscribe(() => {
        this.member.photos = this.member.photos.filter(x => x.id !== photoId);
      });
    }else if(this.uploadType === "member"){
      this.membersService.deletePhoto(photoId).subscribe(() => {
        this.member.photos = this.member.photos.filter(x => x.id !== photoId);
      });
    }

  }

  initializeUpdloader() {
    console.log("member");
    console.log(this.member);
    var endPoint = "users/add-photo";
    if(this.uploadType === "org"){
      endPoint = "organizations/add-photo/"+this.member.id;
    }
    this.uploader = new FileUploader({
      url: this.baseUrl + endPoint,
      authToken: 'Bearer ' + this.user.token,
      isHTML5: true,
      allowedFileType: ['image'],
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });
    console.log(this.uploader);
    console.log(this.baseUrl+endPoint);

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onErrorItem = (item, response, status, headers) => {
      console.log(response);
    }

    

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      console.log("response");
      console.log(response);
      if (response) {
        const photo: Photo = JSON.parse(response);
        this.member.photos.push(photo);
        if (photo.isMain) {
          this.user.photoUrl = photo.url;
          this.member.photoUrl = photo.url;
          this.accountService.setCurrentUser(this.user);
          // this.orgService.set
        }
      }
    } 
  }

}
