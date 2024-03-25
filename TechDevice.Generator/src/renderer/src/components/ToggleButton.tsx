/* eslint-disable prettier/prettier */
import React from 'react'
import { ToggleButton } from 'primereact/togglebutton';
import "primereact/resources/themes/lara-light-cyan/theme.css";

class MyToggleButton extends React.Component<
    { onChange: (value: boolean) => void }, 
    { checked: boolean }
> {
    public constructor(props: { onChange: (value: boolean) => void }) {
        super(props);
        this.state = { checked: false }
    }
    public override render(): React.ReactElement {
        return (
            <div>
                <ToggleButton onLabel="Отключить" offLabel="Включить" 
                    checked={this.state.checked}
                    onChange={(args) => {
                        this.setState({
                            checked: args.value
                        })
                        this.props.onChange(args.value)
                    }} />
            </div>
        );
    }
}
export default MyToggleButton;