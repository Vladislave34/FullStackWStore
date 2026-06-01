import {useRouter} from "next/navigation";
import {useGoogleLogin} from "@react-oauth/google";
import {useAppDispatch} from "@/hooks/redux";
import {loginSuccess} from "@/store/reducers/authSlice";
import {useLoginByGoogleMutation} from "@/services/authService";

const LogButton = () => {
    const[ loginByGoogle ] = useLoginByGoogleMutation();
    const dispatch = useAppDispatch();
    const navigate = useRouter();
    const loginUseGoogle = useGoogleLogin({
        onSuccess: async (tokenResponse) =>
        {
            console.log("tokenResponse", tokenResponse.access_token);
            try {
                const response = await loginByGoogle(tokenResponse.access_token).unwrap();



                dispatch(loginSuccess(response));
                navigate.push('/')
            } catch (error) {
                console.error("Google логін не вдалий:", error);
            }
        },
    });
    return (
        <div>
            <button
                onClick={(event) => {
                    event.preventDefault();
                    loginUseGoogle();
                }}
                className="bg-blue-500 hover:bg-blue-600 transition text-white font-semibold px-4 py-2 rounded w-full mt-4"
            >
                {'LoginGoogle'}
            </button>
        </div>
    );
};

export default LogButton;