"use client"
import EditStoreForm from "@/app/[lng]/UI/forms/EditStoreForm";
import {storeApi} from "@/services/storeService";
import {useDispatch} from "react-redux";


const Page = () => {
    const {data, isLoading} = storeApi.useGetStoreQuery();
    const [editStore] = storeApi.useEditStoreMutation();
    const store = data;
    if(store=== undefined ) {
        return null;
    }

    return (
        <div className="p-8 flex justify-center items-center min-h-screen ">
            <EditStoreForm
                store={store}  // твій IStore об'єкт
                isLoading={isLoading}
                onSubmit={async (values) => {
                    await editStore(values).unwrap();
                }}
            />
        </div>
    );
};

export default Page;