'use client'


const TitleButton = ({func, title}  :{func : ()=>void, title : string }) => {
    return (
        <button
            onClick={func}
            className="
                        h-[42px] px-4
                        bg-[var(--btn)] text-white text-sm font-semibold
                        rounded-lg hover:opacity-90 transition-opacity
                        flex items-center gap-2 whitespace-nowrap
                    "
        >
            {title}
        </button>
    );
};

export default TitleButton;