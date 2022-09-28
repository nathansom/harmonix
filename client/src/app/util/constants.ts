export interface Gender {
    value: string;
    display: string;
}

export interface MultiselectSelectionItem {
    item_id: number;
    item_text: string;
}

export const GENDER_LIST: MultiselectSelectionItem[] = [
    {
        item_id: 1,
        item_text: 'Male',
    },
    {
        item_id: 2,
        item_text: 'Female',
    },
    {
        item_id: 3,
        item_text: 'Non-Binary',
    },
    {
        item_id: 4,
        item_text: 'Unidentified',
    },
    {
        item_id: 5,
        item_text: 'Other',
    },
];

export const SKILL_LIST: MultiselectSelectionItem[] = [
    {
        item_id: 1,
        item_text: 'Skill A',
    },
    {
        item_id: 2,
        item_text: 'Skill B',
    },
    {
        item_id: 3,
        item_text: 'Skill C',
    },
    {
        item_id: 4,
        item_text: 'Skill D',
    },
    {
        item_id: 5,
        item_text: 'Skill E',
    },
];

export const GENRE_LIST: MultiselectSelectionItem[] = [
    {
        item_id: 1,
        item_text: 'Genre A',
    },
    {
        item_id: 2,
        item_text: 'Genre B',
    },
    {
        item_id: 3,
        item_text: 'Genre C',
    },
    {
        item_id: 4,
        item_text: 'Genre D',
    },
    {
        item_id: 5,
        item_text: 'Genre E',
    },
];

// Mock Org data. Real data will reflect OrganizationDto.cs
export const AFFILIATION_DATA = [
  {
    id: 1,
    name: 'Spring Reed',
    introduction: 'Lorem ipsum dolor sit amet',
    established: 2008,
    photos: [
      {
        url: 'https://randomuser.me/api/portraits/women/54.jpg',
        isMain: true,
      },
    ],
  },
  {
    id: 2,
    name: 'Summer Salt',
    introduction: 'Lorem ipsum dolor sit amet',
    established: 2004,
    photos: [
      {
        url: 'https://randomuser.me/api/portraits/women/54.jpg',
        isMain: true,
      },
    ],
  },
  {
    id: 3,
    name: 'Winterizers',
    introduction: 'Lorem ipsum dolor sit amet',
    established: 2012,
    photos: [
      {
        url: 'https://randomuser.me/api/portraits/women/54.jpg',
        isMain: true,
      },
    ],
  }];

  export const AFFILIATION_LIST: MultiselectSelectionItem[] = AFFILIATION_DATA.map( org => ({item_id: org.id, item_text: org.name}));