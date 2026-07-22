export default interface IResetPasswordModel {
    email: string;
    token: string;
    newPassword: string;
    confirmNewPassword: string;
}