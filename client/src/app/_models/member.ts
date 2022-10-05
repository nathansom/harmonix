import { Affiliation } from "./affiliation";
import { Job } from "./job";
import { Organization } from "./organization"; // delete when ready
import { Photo } from "./photo"

export interface Member {
    id: number;
    username: string; // changes required to remove
    photoUrl: string;
    age: number;
    knownAs: string;
    created: Date;
    lastActive: Date;
    gender: string;
    introduction: string; // changes required to remove
    lookingFor: string; // changes required to remove
    interests: string; // changes required to remove
    city: string;
    provinceOrState: string;
    country: string;
    photos: Photo[];
    createdJobs: Job[];
    savedJobs: Job[];
    affiliation: Affiliation[]; // !Possible cycle detection -> delete when ready
    // affiliation: Affiliation[]; 
    // Uncomment the line above when ready
    // Or if no codes depend on accessing nested organization objects 
    // within Member object

    firstName: string;
    lastName: string;

    email: string;


    occupation: string;
    skills: string;
    genres: string;

   
    
    

}


