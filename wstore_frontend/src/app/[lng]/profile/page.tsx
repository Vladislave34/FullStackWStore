import {redirect} from "next/navigation";


const Profile = async ({params} : {params: Promise<{lng:string}>}) => {
    const {lng} = await params
    redirect(`/${lng}/profile/details`)
};

export default Profile;