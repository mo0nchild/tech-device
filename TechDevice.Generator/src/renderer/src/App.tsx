/* eslint-disable @typescript-eslint/ban-types */
/* eslint-disable prettier/prettier */
import React from 'react'
import GeneratorKnob from './components/GeneratorKnob'
import MyToggleButton from './components/ToggleButton'
import './App.css'

export type AppState = {
    readonly voltage: number;
    readonly amperage: number;
}
export type ServerState = [boolean, React.Dispatch<React.SetStateAction<boolean>>];
class App extends React.Component<{}, AppState> {
    public constructor(props: {}) {
        super(props);
        this.state = { voltage: 0, amperage: 0};
    }
    private generatorChanged = (state: AppState): void => {
        window.api.invoke.getData(state);
        this.setState({
            amperage: state.amperage,
            voltage: state.voltage, 
        });
    };
    public override render(): React.ReactElement {
        return (
            <div>
                <div className='main-content'>
                    <div className='device-items'>
                        <GeneratorKnob componentName={'Сила тока'} 
                            onValueChange={(value) => this.generatorChanged({
                                amperage: value,
                                voltage: this.state.voltage,
                            })}
                            labelFormater={(value: string) => `${value} А`}
                            maximum={5} minimum={0} step={0.5}/>
                    </div>
                    <div className='device-items'>
                        <GeneratorKnob componentName={'Напряжение'} 
                            onValueChange={(value) => this.generatorChanged({
                                amperage: this.state.amperage,
                                voltage: value,
                            })}
                            labelFormater={(value: string) => `${value} В`}
                            maximum={220} minimum={0} step={20}/>
                    </div>
                </div>
                <div style={{
                    display: 'flex',
                    flexDirection: 'row',
                    justifyContent: 'center'
                }}>
                    <MyToggleButton onChange={(value) => {
                        if(value === true) {
                            window.api.invoke.serverUp();
                        }
                        else window.api.invoke.serverDown();
                    }}/>
                </div>
            </div>
        );
    }
}
export default App;
