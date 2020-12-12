export interface ContactModel{
    contactType: number;
    id: number;
    name: string;
    phoneNumber: string;
    birthDate: Date;
    createdDate: Date;
    isFavorite: boolean;
    rating: number;
    editorData: string;
    userId: string;
}