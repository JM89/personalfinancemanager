import type {ReactNode} from 'react';
import Link from '@docusaurus/Link';
import useBaseUrl from '@docusaurus/useBaseUrl';
import {useDoc} from '@docusaurus/plugin-content-docs/client';
import DocItemContent from '@theme-original/DocItem/Content';
import Heading from '@theme/Heading';
import type {Props} from '@theme/DocItem/Content';

import styles from './styles.module.css';

function WorkspaceWalkthrough() {
  const poster = useBaseUrl('/img/pfm/react-expense-types.png');
  const video = useBaseUrl('/video/pfm-workspace.webm');
  const captions = useBaseUrl('/video/pfm-workspace.en.vtt');

  return (
    <section className="markdown">
      <Heading as="h2">See it in action</Heading>
      <p>
        This silent, captioned walkthrough covers the dashboard, theme switching,
        configuration tables, category forms and safe deletion. No data is saved or
        deleted during the tour.
      </p>
      <video
        controls
        playsInline
        preload="none"
        poster={poster}
        className={styles.video}
        aria-label="PFM workspace walkthrough">
        <source src={video} type="video/webm" />
        <track default kind="captions" src={captions} srcLang="en" label="English" />
        Your browser does not support embedded video.
      </video>
      <p>
        <a href={video}>Download the walkthrough</a> or follow the{' '}
        <Link to="/docs/Development/overview#workspace-walkthrough">text walkthrough</Link>.
      </p>
    </section>
  );
}

export default function DocItemContentWrapper(props: Props): ReactNode {
  const {metadata} = useDoc();

  return (
    <>
      <DocItemContent {...props} />
      {metadata.source === '@site/docs/0000-overview.md' && <WorkspaceWalkthrough />}
    </>
  );
}
