export default interface IRegisterModel {
    firstName: string;
    lastName: string;
    image: File | null;
    username: string;
    email: string;
    password: string;
}