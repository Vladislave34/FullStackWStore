

const SaleLabel = ({sale}: {sale:string | null}) => {
    if (Number(sale) === 0) return null;
    return (
        <div style={{
            background: "var(--sale)",
            color: "var(--text)"
        }}
            className="rounded-lg absolute top-2 left-2 px-2 py-1 text-sm font-medium">
        -{sale}%
        </div>
    );
};

export default SaleLabel;