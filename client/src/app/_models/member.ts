import { Photo } from "./photo"

export interface Member {
    id: number;
    username: string; // changes required to remove
    introduction: string; // changes required to remove
    lookingFor: string; // changes required to remove
    interests: string; // changes required to remove
    firstName: string;
    lastName: string;
    knownAs: string;
    email: string;
    gender: string;
    age: string;
    city: string;
    provinceOrState: string;
    country: string;
    occupation: string;
    skills: string[];
    genres: string[];
    affiliation: string[];
    created: Date;
    lastActive: Date;
    photoUrl: string;
    photos: Photo[];
}


