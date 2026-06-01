"use client"
import EditStoreForm from "@/UI/forms/EditStoreForm";
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
        <div className="p-6 flex justify-center items-center min-h-screen w-[85%]">
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