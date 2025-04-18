import React, { useCallback } from 'react';
import { Handle, Position } from '@xyflow/react';

const handleStyle = { top: 10 };

export default function NoiseNode({ data }) {
    const onChange = useCallback((evt) => {
        console.log(evt.target.value);
    }, []);

    return (
        <div style={{backgroundColor: "black", padding: 10, width:120}}>
            <p>Noise Node</p>
            <Handle type="target" position={Position.Left} />
            <Handle type="source" position={Position.Right} id="a"/>
            <Handle type="source" position={Position.Right} id="b" style={handleStyle} />
        </div>
    );
}
